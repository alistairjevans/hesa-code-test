import React, { Component } from 'react';
import authService from './api-authorization/AuthorizeService'

export class UserSearch extends Component {
  static displayName = UserSearch.name;

  timerId = null;

  constructor(props) {
    super(props);
    this.state = { enteredLastName: '', users: [], loading: false };

    this.handleEnteredName = this.handleEnteredName.bind(this);
  }

  static renderUsers(users, enteredName) {

    if (users.length)
    {
      return (
        <table className='table table-striped' aria-labelledby="tableLabel">
          <thead>
            <tr>
              <th>Username</th>
              <th>First Name</th>
              <th>Last Name</th>
              <th>Email</th>
            </tr>
          </thead>
          <tbody>
            {users.map(user =>
              <tr key={user.userId}>
                <td>{user.userName}</td>
                <td>{user.firstName}</td>
                <td>{user.lastName}</td>
                <td>{user.emailAddress}</td>
              </tr>
            )}
          </tbody>
        </table>
      );
    }
    else if (enteredName && enteredName.trim())
    {
      return <p>No user found with the last name '{enteredName}'.</p>
    }
    else
    {
      return (
        <div>
          <p>Enter the last name of a user to see results.</p>
          <div>
            <i>Tip:</i> Some example last names in the test seed data:
              <ul>
                <li>Bloggs</li>
                <li>Hanley</li>
                <li>Luney</li>
              </ul>
          </div>
          <div>
            The full list of users that have been seeded can be found in the
            <i> seed_users.json </i> file in the Hesa.Logic project.
          </div>
        </div>
      );
    }
  }

  handleEnteredName(event) {

    var enteredValue = event.target.value;

    this.setState({ enteredLastName: enteredValue });

    if (this.timerId) {
      clearTimeout(this.timerId);
    }

    // Kick off a timer to fetch the results (to allow for typing delay)
    this.timerId = setTimeout(() => {
      if (enteredValue && enteredValue.trim()) {
        this.loadUserResults(enteredValue);
      }
      else
      {
        this.setState({ users: [] });
      }
      this.timerId = null;
    }, 400);
  }

  render() {

    let userSearchResults = UserSearch.renderUsers(this.state.users, this.state.enteredLastName);

    return (
      <div>
        <h1 id="tableLabel" >User Search</h1>
        <p>
          <input name="userLastName" value={this.state.enteredLastName} onChange={this.handleEnteredName} placeholder="Last Name" />
        </p>
        <hr />
        {userSearchResults}
      </div>
    );
  }

  async loadUserResults(enteredName) {    
    this.setState({ loading: true });
    const token = await authService.getAccessToken();
    const response = await fetch(`/api/userSearch/searchByLastName?lastName=${enteredName}`, {
      headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
    });
    const data = await response.json();
    this.setState({ users: data, loading: false });
  }
}
