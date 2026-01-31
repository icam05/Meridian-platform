import axios from "axios";

export const api = axios.create({
    baseURL: "http://localhost:5242", // your ASP.NET port
    headers: {
        "Content-Type": "application/json",
    },
});
