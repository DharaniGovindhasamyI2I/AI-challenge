// src/database.js
export function findUser(id) {
  // Simulate DB lookup
  return { id, name: 'Real User' };
}

export function createUser(user) {
  // Simulate DB insert
  return { id: Date.now(), ...user };
} 