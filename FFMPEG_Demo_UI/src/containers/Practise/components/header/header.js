import React from "react";
import { Link } from "react-router-dom";
import "./header.scss";
import { ReactComponent as Logo } from "../../assets/crown.svg";

function Header() {
  return (
    <div className="header">
      <Link className="logo-container" to="/practise">
        <Logo className="logo" />
      </Link>
      <div className="options">
        <Link className="option" to="/practise/shop">
            Shop
        </Link>
        <Link className="option" to="/practise/contact">
            Contact
        </Link>
        <Link className="option" to="/practise/signin">
            Sign In
        </Link>
      </div>
    </div>
  );
}

export default Header;
