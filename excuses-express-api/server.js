import dotenv from "dotenv";
import fetch from "node-fetch";
import https from "https";
import express from "express";

dotenv.config();

const PORT = process.env.PORT || 3000;
const NODE_ENV = process.env.NODE_ENV || "development";
const DOTNET_API_URL = NODE_ENV === "production" ? process.env.API_URL : "https://localhost:7180/api/v1/excuses";

const app = express();
app.use(express.static("public"));
app.use(express.json());

app.post("/api/excuses", async (req, res) => {
  try {
    const response = await fetch(DOTNET_API_URL, {
      method: "POST",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify(req.body),
      agent: NODE_ENV === "production" ? undefined : new https.Agent({
        rejectUnauthorized: false
      })
    });

    if (!response.ok) {
      res.status(500).json({message: "Failed to create excuse"});
    }

    const data = await response.json();
    res.json(data);
  } catch (err) {
    res.status(500).json({message: err});
  }
});

app.get("/api/excuses", async (req, res) => {
  try {
    const {category} = req.query;
    const response = await fetch(category ? `${DOTNET_API_URL}?category=${category}` : DOTNET_API_URL, {
      agent: NODE_ENV === "production" ? undefined : new https.Agent({
        rejectUnauthorized: false
      })
    });

    if (!response.ok) {
      res.status(500).json({message: "Failed to fetch excuses"});
    }

    const data = await response.json();
    res.json(data);
  } catch (err) {
    res.status(500).json({message: err});
  }
});

app.get("/api/excuses/random", async (req, res) => {
  try {
    const {category} = req.query;
    const response = await fetch(category ? `${DOTNET_API_URL}/random?category=${category}` : `${DOTNET_API_URL}/random`, {
      agent: NODE_ENV === "production" ? undefined : new https.Agent({
        rejectUnauthorized: false
      })
    });

    if (response.ok) {
      const data = await response.json();
      return res.json(data);
    }

    res.status(500).json({message: "Failed to fetch random excuse"});
  } catch (err) {
    res.status(500).json({message: err});
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
    const response = await fetch(`${DOTNET_API_URL}/${req.params.id}`, {
      agent: NODE_ENV === "production" ? undefined : new https.Agent({
        rejectUnauthorized: false
      })
    });

    if (response.ok) {
      const data = await response.json();
      return res.json(data);
    } else if (response.status === 404) {
      return res.status(404).json({message: "Excuse not found"});
    }

    res.status(500).json({message: "Failed to fetch excuse"});
  } catch (err) {
    res.status(500).json({message: err});
  }
});

app.get("/api/excuses/categories", async (req, res) => {
  try {
    const response = await fetch(`${DOTNET_API_URL}/categories`, {
      agent: NODE_ENV === "production" ? undefined : new https.Agent({
        rejectUnauthorized: false
      })
    });

    if (response.ok) {
      const data = await response.json();
      return res.json(data);
    }

    res.status(500).json({message: "Failed to fetch categories"});
  } catch (err) {
    res.status(500).json({message: err});
  }
});

app.put("/api/excuses/:id", async (req, res) => {
  const id = parseInt(req.params.id);
  if (isNaN(id)) {
    return res.status(400).json({error: "Invalid ID format"});
  }

  try {
    const response = await fetch(`${DOTNET_API_URL}/${req.params.id}`, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify(req.body),
      agent: NODE_ENV === "production" ? undefined : new https.Agent({
        rejectUnauthorized: false
      })
    });

    if (response.ok) {
      return res.status(204).send();
    } else if (response.status === 404) {
      return res.status(404).json({message: "Excuse not found"});
    }

    res.status(500).json({message: "Failed to update excuse"});
  } catch (err) {
    res.status(500).json({message: err});
  }
});

app.delete("/api/excuses/:id", async (req, res) => {
  const id = parseInt(req.params.id);
  if (isNaN(id)) {
    return res.status(400).json({error: "Invalid ID format"});
  }

  try {
    const response = await fetch(`${DOTNET_API_URL}/${req.params.id}`, {
      method: "DELETE",
      agent: NODE_ENV === "production" ? undefined : new https.Agent({
        rejectUnauthorized: false
      })
    });

    if (response.ok) {
      return res.status(204).send();
    } else if (response.status === 404) {
      return res.status(404).json({message: "Excuse not found"});
    }

    res.status(500).json({message: "Failed to delete excuse"});
  } catch (err) {
    res.status(500).json({message: err});
  }
});

app.listen(PORT, () => {
  console.log(`Server is running on port ${PORT}`);
});
