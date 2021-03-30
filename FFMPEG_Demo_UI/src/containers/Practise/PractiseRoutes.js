import React, { Fragment } from "react";
import { Route, Switch, Link } from "react-router-dom";

import ShopPage from "./pages/shop/shop";
import SigninSignup from "./pages/signin-signup/signin-signup";

export function PractiseRoutes() {
  return (
    <Fragment>
      <Route path="/practise/shop" exact component={ShopPage} />
      <Route path="/practise/signin" exact component={SigninSignup} />
    </Fragment>
  );
}

export function PractiseLinks() {
  return (
    <Fragment>
    </Fragment>
  );
}

