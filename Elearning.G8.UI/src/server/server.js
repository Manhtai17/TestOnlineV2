const jsonServer = require("json-server");
const auth = require("json-server-auth");
const cors = require("cors");
const server = jsonServer.create();
const router = jsonServer.router("src/json/db.json");
const middlewares = jsonServer.defaults();

server.db = router.db;

server.use(cors());
server.use(auth);
server.use(middlewares);
server.use(router);
server.listen(3001, () => {
	console.log("JSON Server is running");
});
