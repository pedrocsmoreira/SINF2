var mongoose = require('mongoose');
var Prato = require('./pratos');
var Schema = mongoose.Schema;

var EmentaSchema   = new Schema({
    id: {
        type: Schema.Types.ObjectId
    },
    data: {
        type: Date
    },
    listaPratos: {
        type: [Prato.schema]
    }
});

module.exports = mongoose.model('Ementa', EmentaSchema);