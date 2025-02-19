import axios from "axios";
import {configDotenv} from "dotenv";
import express from "express";
// import https from "https";

configDotenv();

const app = express();
app.use(express.static("public"));
app.use(express.json());

const PORT = process.env.PORT || 3000;

// !: For deploying on AWS. Use HTTP not HTTPS.
// const DOTNET_API_URL = process.env.DOTNET_API_URL || "http://localhost:5004/api/v1";

// !: For deploying on local machine
const DOTNET_API_URL = process.env.DOTNET_API_URL || "http://localhost:5004/api/v1";
const client = axios.create({});

// !: Use when running .NET API locally with https
// const DOTNET_API_URL = process.env.DOTNET_API_URL || "https://localhost:7180/api/v1";
// const client = axios.create({
//   httpsAgent: new https.Agent({
//     rejectUnauthorized: false,
//   }),
// });

app.post("/api/excuses", async (req, res) => {
  try {
    const response = await client.post(`${DOTNET_API_URL}/excuses`, req.body);
    res.json(response.data);
  } catch (err) {
    handleAxiosError(err, res);
  }
});

app.get("/api/excuses", async (req, res) => {
  try {
    const {category} = req.query;
    const response = await client.get(category ? `${DOTNET_API_URL}/excuses?category=${category}` : `${DOTNET_API_URL}/excuses`);
    res.json(response.data);
  } catch (err) {
    handleAxiosError(err, res);
  }
});

app.get("/api/excuses/random", async (req, res) => {
  try {
    const {category} = req.query;
    const response = await client.get(category ? `${DOTNET_API_URL}/excuses/random?category=${category}` : `${DOTNET_API_URL}/excuses/random`);
    res.json(response.data);
  } catch (err) {
    handleAxiosError(err, res);
  }
});

// NOTE: Order matters. This endpoint must come after the "/api/excuses/random" endpoint
// for express to be able to handle both endpoints correctly.
app.get("/api/excuses/:id", async (req, res) => {
  const id = parseInt(req.params.id);
  if (isNaN(id)) {
    return res.status(400).json({error: "Invalid ID format"});
  }

  try {
    const response = await client.get(`${DOTNET_API_URL}/excuses/${id}`);
    res.json(response.data);
  } catch (err) {
    handleAxiosError(err, res);
  }
});

app.get("/api/excuses/categories", async (req, res) => {
  try {
    const response = await client.get(`${DOTNET_API_URL}/excuses/categories`);
    res.json(response.data);
  } catch (err) {
    handleAxiosError(err, res);
  }
});

app.put("/api/excuses/:id", async (req, res) => {
  const id = parseInt(req.params.id);
  if (isNaN(id)) {
    return res.status(400).json({error: "Invalid ID format"});
  }

  try {
    const response = await client.put(`${DOTNET_API_URL}/excuses/${id}`);
    res.json(response.data);
  } catch (err) {
    handleAxiosError(err, res);
  }
});

app.delete("/api/excuses/:id", async (req, res) => {
  const id = parseInt(req.params.id);
  if (isNaN(id)) {
    return res.status(400).json({error: "Invalid ID format"});
  }

  try {
    const response = await client.delete(`${DOTNET_API_URL}/excuses/${id}`);
    res.json(response.data);
  } catch (err) {
    handleAxiosError(err, res);
  }
});

function handleAxiosError(error, res) {
  if (error.response) {
    if (error.response.status === 404) {
      res.status(404).json({error: "Excuse not found"});
    } else {
      res.status(error.response.status).json({error: error.response.data});
    }
  } else if (error.request) {
    res.status(500).json({error: "No response received from .NET API"});
  } else {
    res.status(500).json({error: "Unexpected error occurred"});
  }
}

app.listen(PORT, () => {
  console.log(`Server is running on port ${PORT}`);
});
