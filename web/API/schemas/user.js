const { Schema, model } = require('mongoose')
const bcrypt = require('bcrypt')
const { isEmail } = require('validator')
const Joi = require('joi')

var UserSchema = new Schema({
    username: {
        type: String,
        required: true,
        unique: true,
        minlength: 3,
        maxlength: 10
    },
    email: {
        type: String,
        required: true,
        unique: true,
        lowercase: true,
        validate: {
            validator: email => isEmail(email),
            message: '{VALUE} is not a valid email'
        }
    },
    password: {
        type: String,
        required: true,
        minlength: 4
    },
    createdAt: {
        type: Date
    }
})

UserSchema.pre('save', function(next) {
    var user = this
    if (!user.isModified('password')) return next()
    bcrypt.genSalt(10, function(err, salt) {
        if (err) return next(err)
        bcrypt.hash(user.password, salt, function(err, hash) {
            if (err) return next(err)
            user.password = hash
            user.createdAt = Date.now()
            next()
        })
    })
})

UserSchema.methods.comparePassword = function(candidatePassword, cb) {
    bcrypt.compare(candidatePassword, this.password, function(err, isMatch) {
        if (err) return cb(err)
        cb(null, isMatch)
    })
}

module.exports.User = model('User', UserSchema)
module.exports.Validate = (data) => {
    return Joi.object({
        username: Joi.string()
        .alphanum()
        .min(3)
        .max(10)
        .required(),
        password: Joi.string()
        .min(4)
        .required()
    }).validate(data)
}