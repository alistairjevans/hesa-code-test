import React, { Component } from 'react';

export class Home extends Component {
  static displayName = Home.name;

  render () {
    return (
      <div>
        <h1>Alistair Evans - HESA Code Test</h1>

        <div>
          This application is a Single-Page-Application (SPA), built using:
          <ul>
            <li>ASP.NET Core Identity (with an EF Core back-end).</li>
            <li>IdentityServer as a server-side OIDC identity provider.</li>
            <li>React as the front-end library.</li>
            <li>Bootstrap for some basic layout.</li>
          </ul>
        </div>

        <p>
          To see the User Search function, you need to be logged in as an Administrator.
          If you are logged out, or logged in as a non-admin role, then you will not see the menu item.

          The admin user details are displayed on the login page (very secure!) for convenience.          
        </p>

        <div>
          The application is based on the default SPA Identity Server template for ASP.NET Core,
          but I've added:

          <ul>
            <li>Policy-Based Authorisation from token claims (backed by Identity Roles)</li>
            <li>Customised Login and Registration pages</li>
            <li>Separated api-server, logic and data projects.</li>
            <li>Menu item hiding based on role.</li>
          </ul>
        </div>
      </div>
    );
  }
}
