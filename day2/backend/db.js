const mongoose = require('mongoose')

mongoose.connect("<include your database link here>")


const todoSchema = mongoose.Schema( {
    title: String,
    description: String,
    completed: Boolean
})

const todo = mongoose.model('todos',todoSchema)

module.exports = {
    todo
}
