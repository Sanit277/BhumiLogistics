import { useState } from "react";
import { isLoggedIn } from "./api/client";
import LoginPage from "./pages/LoginPage";
import DashboardPage from "./pages/DashboardPage";
import "./styles.css";

export default function App() {
  const [loggedIn, setLoggedIn] = useState(isLoggedIn());

  return loggedIn ? (
    <DashboardPage onSignedOut={() => setLoggedIn(false)} />
  ) : (
    <LoginPage onLoggedIn={() => setLoggedIn(true)} />
  );
}
