const { Router } = require("express")
const jwt = require('jsonwebtoken')
const router = Router()

const { User, Validate } = require('../schemas/user')
const resp = require('../src/resp')

router.post('/login', (req, res) => {
    console.log(req.body)
    const { error } = Validate(req.body);
    if (error) return res.status(400).json(resp(false, error.details[0].message))
    User.findOne({ username: req.body.username }, (err, user) => {
        if (err) return res.status(500).json(resp(false, "Internal error"))
        if (!user) return res.status(404).json(resp(false, "User not found"))
        user.comparePassword(req.body.password, (err, isMatch) => {
            if (err) return res.status(500).json(resp(false, "Internal error"))
            if (isMatch) return res.json(resp(true, {
                token: jwt.sign({ id: user._id.toString() }, process.env.JWT, {
                    expiresIn: "14d"
                })
            }))
            res.status(403).json(resp(false, "Invalid password"))
        })
    })
})

router.post('/register',async(req, res) => {
    let user = new User(req.body)
    user.save((err, data) => {
        if (err) return res.status(500).json(resp(false, err.message))
        res.json(resp(true, {
            token: jwt.sign({ id: data._id.toString() }, process.env.JWT, {
                expiresIn: "14d"
            })
        }))
    })
})

module.exports = router