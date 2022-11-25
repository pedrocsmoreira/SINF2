var express = require('express');
var app = express();
var bodyParser = require('body-parser');
var mongoose = require('mongoose')
var cors = require('cors');

app.use(cors());
app.use(bodyParser.urlencoded({ extended: true }));
app.use(bodyParser.json());

const port = process.env.PORT || 8080;

app.listen(port);
console.log('Server ready on port ' + port);

mongoose.connect('mongodb+srv://root:root@sinf2.usuxbum.mongodb.net/?retryWrites=true&w=majority',
    () => console.log('Database Connected')
);

var router = express.Router();

app.use('/api', router);

router.get('/', function(req, res) {
    res.json({ message: 'hooray! welcome to our api!' });
});

const pratosRoute = require('./src/routes/pratos');
const ementasRoute = require('./src/routes/ementas');
const reservasRoute = require('./src/routes/reservas');

app.use('/api/pratos', pratosRoute);
app.use('/api/ementas', ementasRoute);
app.use('/api/reservas', reservasRoute);

module.exports = router;
