import React from "react";
import { Route, Switch, Link } from "react-router-dom";
import styled from "styled-components";
import { useHistory } from "react-router-dom";

import Book from "./containers/Book/Book";
import Practise from "./containers/Practise/Practise";
import {
  PractiseLinks,
  PractiseRoutes,
} from "./containers/Practise/PractiseRoutes";
import Header from "./containers/Practise/components/header/header";

const Welcome = styled.div`
  display: flex;
  justify-content: center;
  align-items: center;
  height: 70vh;
  font-size: 2rem;
  color: #bbb2b2;
  text-shadow: 1px 2px 1px #03a9f4;
  flex-direction: column;
  a {
    font-size: 1rem;
    text-decoration: none;
    margin-right: 1rem;
  }
`;

export default function Routes(props) {
  let history = useHistory();
  return (
    <>
      <Route path="/" exact>
        <Welcome>
          <div>Welcome to geneo Book player</div>
          <div>
            <Link to="/practise">Practise</Link>
            <PractiseLinks />
            <Link to="/books">Books</Link>
          </div>
        </Welcome>
      </Route>
      <Route path="/practise" component={Header} />
      <Switch>
        <Route path="/books" exact component={Book} />
        <Route path="/practise" exact component={Practise} />
        <PractiseRoutes />
      </Switch>
    </>
  );
}
