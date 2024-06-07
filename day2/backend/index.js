const express = require('express')
const { createTodo, updateTodo } = require("./types")
const { todo } = require("./db")
const app = express()

app.use(express.json())

app.post("/todo", async (req, res) => {
    const createPayload = req.body
    const parsedPayload = createTodo.safeParse(createPayload)
    if (!parsedPayload.success) {
        res.status(400).json({
            msg: "u send the wrong inputs."
        })
        return;
    }
    await todo.create({
        title: parsedPayload.data.title,
        description: parsedPayload.data.description,
        completed: false
    })

    res.json({
        msg: "Todo Created Successfully."
    })
})

app.get("/todos", async (req, res) => {
    const todos = await todo.find({})
    res.json({
        todos
    })
})

app.put("/completed", async (req, res) => {
    const updatePayload = req.body
    const parsedPayload = updateTodo.safeParse(updatePayload)
    if (!parsedPayload.success) {
        res.status(400).json({
            msg: "u send the wrong inputs."
        })
        return;
    }
    await todo.updateOne({
        _id: req.body.id
    }, {
        completed: true
    })
    res.json({
        msg: "todo completed"
    })
})

app.listen(3000, () => {
    console.log('Server is running on port 3000');
})
