const express = require('express');
const router = express.Router();

const Ementa = require('../models/ementas');
const Prato = require('../models/pratos');

router.get('/', async (req, res) => {
    try {
        const ementas = await Ementa.find();
        res.json(ementas);
    }catch (err) {
        res.json({ message: err });
    }
});

router.get('/:ementaId', async (req, res) => {
    try {
        const ementa = await Ementa.findById(req.params.ementaId);
        res.json(ementa);
    }catch (err) {
        res.json({ message: err });
    }
});

router.post('/', async (req, res) => {
    var prato = new Prato();

    const data = req.body.data.split("/");

    var ementa = new Ementa();

    ementa.data = new Date(data[2], data[1], data[0]);

    const pratos = req.body.listaPratos.split("/");
    for(let i = 0; i < pratos.length; i++){
        prato = await Prato.findById(pratos[i]);
        ementa.listaPratos.push(prato);
    }

    try {
        const savedEmenta = await ementa.save();
        res.json(savedEmenta);
    }catch (err) {
        res.json({ message: err });
    }
});

router.delete('/:ementaId', async (req, res) => {
    try {
        const removedEmenta = await Ementa.remove({ _id: req.params.ementaId });
        res.json(removedEmenta);
    }catch (err) {
        res.json({ message: err });
    }
});

module.exports = router;