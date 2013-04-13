
/**
 */
var express = require("express"),
    server = express(),
    fs = require('fs');
    port = parseInt(process.env.PORT, 10) || 4567;
server.configure(function(){
  server.use(express.methodOverride());
  server.use(express.bodyParser({uploadDir: 'public/uploads'}));
  server.use(express.static(__dirname + '/public'));
  server.use(express.errorHandler({
    dumpExceptions: true,
    showStack: true
  }));
  server.use(server.router);
});
/*
 * redirects to Index.html
 */
server.get("/", function(request, response) {
  response.redirect("/index.html");
});

server.listen(port);

