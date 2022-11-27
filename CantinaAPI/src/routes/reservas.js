const express = require('express');
const router = express.Router();

const Prato = require('../models/pratos');
const Reserva = require('../models/reservas');

const restClient = require('node-rest-client').Client;

router.get('/', async (req, res) => {
    try {
        const reservas = await Reserva.find();
        res.json(reservas);
    }catch (err) {
        res.json({ message: err });
    }
});

router.get('/:reservaId', async (req, res) => {
    try {
        const reserva = await Reserva.findById(req.params.reservaId);
        res.json(reserva);
    }catch (err) {
        res.json({ message: err });
    }
});

router.post('/', async (req, res) => {
    const prato = await Prato.findById(req.body.pratoId);
    var aluno = new restClient();
    const datas = req.body.data.split("/");
    var saldo = 0;

    var reserva = new Reserva();

    process.env['NODE_TLS_REJECT_UNAUTHORIZED'] = 0;

    aluno.get("htpp://localhost:7000/api/aluno/" + req.body.numAluno, function(request, response){
        reserva.aluno.Num_aluno = req.body.numAluno;
        reserva.aluno.Nome_aluno = request.Nome;
        reserva.aluno.Email_aluno = request.Email;

        saldo = request.Saldo;
    });

    reserva.data = new Date(datas[2], datas[1], datas[0]);
    reserva.pratoReservado = prato;
    reserva.aluno = aluno;

    try{
        reserva.custo = req.body.custo;
    }catch(err){
        reserva.custo = 4;
    }

    if(saldo >= reserva.custo){
        aluno.put("http://localhost:7000/api/aluno/saldo?id=" + req.body.numAluno + "&custo=" + prato.custo, function(){});

        try {
            const savedReserva = await reserva.save();
            res.json(savedReserva);
        }catch (err) {
            res.json({ message: err });
        }
    }else {
        res.json({ message: "saldo insuficiente" });
    }
});

router.delete('/:reservaId', async (req, res) => {z
    try {
        const removedReserva = await Reserva.remove({ _id: req.params.reservaId });

        try {
            const reserva = await Reserva.findById(req.params.reservaId);
            aluno.put("http://localhost:7000/api/aluno/saldo?id=" + reserva.aluno.numAluno + "&custo=-" + reserva.custo, function(){});
            const removedReserva = await Reserva.remove({ _id: req.params.reservaId });
            res.json(removedReserva, {message: "saldo devolvido"});
        }catch (err) {
            res.json({ message: err });
        }

        
    } catch (err) {
        res.json({ message: err });
    }
});

module.exports = router;