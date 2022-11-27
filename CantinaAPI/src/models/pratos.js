var mongoose = require('mongoose');
var Schema = mongoose.Schema;

var PratoSchema = new Schema({
    id: {
        type: Schema.Types.ObjectId
    },
    nome_prato: {
        type: String
    },
    dia_prato: {
        type: String
    }
});

module.exports = mongoose.model('Prato', PratoSchema);