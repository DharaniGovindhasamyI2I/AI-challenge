export function findUser(id) {
  if (id === '123') {
    return {
      id: '123',
      name: 'John Doe',
      email: 'john.doe@example.com',
    };
  }
  return null;
  // Imagine this talks to a real database
}

export function createUser(user) {
  if (user.id === '123') {
    return {
      id: '123',
      name: 'John Doe',
      email: 'john.doe@example.com',
    };
  }
  return null;

  // Imagine this creates a user in a real database
}
