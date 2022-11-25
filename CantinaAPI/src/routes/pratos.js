const express = require('express');
const router = express.Router();

const Prato = require('../models/pratos');

router.get('/', async (req, res) => {
    try {
        const pratos = await Prato.find();
        res.json(pratos);
    } catch (err) {
        res.json({ message: err });
    }
});

router.get('/:pratoId', async (req, res) => {
    try {
        const prato = await Prato.findById(req.params.pratoId);
        res.json(prato);
    } catch (err) {
        res.json({ message: err });
    }
});

router.post('/', async (req, res) => {
    const prato = new Prato();

    prato.nome_prato =  req.body.nome_prato;
    prato.dia_prato = req.body.dia_prato;

    try {
        const savedPrato = await prato.save()
        res.json(savedPrato);
    } catch (err) {
        res.json({ message: err });
    }
});

router.put('/:pratoId', async (req, res) => {
    try {
        var prato = await Prato.findById(req.params.pratoId);

        prato.nome_prato = req.body.nome_prato;
        prato.dia_prato = req.body.dia_prato;

        const changedPrato = await prato.save()
        res.json(changedPrato);
    } catch (err) {
        res.json({ message: err });
    }
});

router.delete('/:pratoId', async (req, res) => {
    try {
        const removedPrato = await Prato.remove({ _id: req.params.pratoId });
        res.json(removedPrato);
    } catch (err) {
        res.json({ message: err });
    }
});

module.exports = router;
