// src/database.test.js
jest.mock('./database'); // Use the manual mock
import { findUser, createUser } from './database';

describe('database module mocks', () => {
  test('should mock findUser', () => {
    findUser.mockReturnValue({ id: 1, name: 'Alice' });
    expect(findUser(1)).toEqual({ id: 1, name: 'Alice' });
  });

  test('should mock createUser', () => {
    createUser.mockReturnValue({ id: 2, name: 'Bob' });
    expect(createUser({ name: 'Bob' })).toEqual({ id: 2, name: 'Bob' });
  });

  test('should mock findUser error', () => {
    findUser.mockImplementation(() => { throw new Error('DB error'); });
    expect(() => findUser(99)).toThrow('DB error');
  });
}); 