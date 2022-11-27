var mongoose = require('mongoose');
var Prato = require('./pratos');
var Schema = mongoose.Schema;

var ReservaSchema   = new Schema({
    id: {
        type: Schema.Types.ObjectId
    },
    data: {
        type: Date
    },
    pratoReservado: {
        type: Prato.schema
    },
    aluno: {
        Num_aluno: {
            type: Number
        },
        Nome_aluno: {
            type: String
        },
        Email_aluno: {
            type: String
        }
    },
    custo: {
        type: Number
    }
});

module.exports = mongoose.model('Reserva', ReservaSchema);