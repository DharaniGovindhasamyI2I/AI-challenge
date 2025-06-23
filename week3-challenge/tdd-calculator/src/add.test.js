import { add } from './add';

describe('add', () => {
  // Success scenarios
  test('should add two positive numbers (AAA)', () => {
    // Arrange
    const a = 2, b = 3;
    // Act
    const result = add(a, b);
    // Assert
    expect(result).toBe(5);
  });

  test('should add negative numbers', () => {
    const result = add(-2, -3);
    expect(result).toBe(-5);
  });

  test('should add zero', () => {
    const result = add(0, 0);
    expect(result).toBe(0);
  });

  test('should add decimal numbers', () => {
    const result = add(1.5, 2.7);
    expect(result).toBeCloseTo(4.2);
  });

  // Failure scenarios
  test('should throw error when first argument is not a number', () => {
    expect(() => add('5', 3)).toThrow('Both arguments must be numbers');
  });

  test('should throw error when second argument is not a number', () => {
    expect(() => add(5, '3')).toThrow('Both arguments must be numbers');
  });

  test('should throw error when arguments are NaN', () => {
    expect(() => add(NaN, 5)).toThrow('Arguments cannot be NaN');
    expect(() => add(5, NaN)).toThrow('Arguments cannot be NaN');
  });

  // Boundary value testing
  test('should add Number.MAX_SAFE_INTEGER and 1', () => {
    const result = add(Number.MAX_SAFE_INTEGER, 1);
    expect(result).toBe(Number.MAX_SAFE_INTEGER + 1);
  });

  test('should add Number.MIN_SAFE_INTEGER and -1', () => {
    const result = add(Number.MIN_SAFE_INTEGER, -1);
    expect(result).toBe(Number.MIN_SAFE_INTEGER - 1);
  });

  test('should throw error when first argument is null', () => {
    expect(() => add(null, 3)).toThrow('Both arguments must be numbers');
  });

  test('should throw error when second argument is undefined', () => {
    expect(() => add(3, undefined)).toThrow('Both arguments must be numbers');
  });

  test('should throw error when both arguments are empty strings', () => {
    expect(() => add('', '')).toThrow('Both arguments must be numbers');
  });

  test('should handle extremely large numbers', () => {
    const result = add(1e308, 1e308);
    expect(result).toBe(Infinity); // JS behavior
  });

  test('should throw error when arguments are objects', () => {
    expect(() => add({}, {})).toThrow('Both arguments must be numbers');
  });

  test('should throw error when arguments are arrays', () => {
    expect(() => add([], [])).toThrow('Both arguments must be numbers');
  });
});


// import request from 'supertest';
// import app from './app'; // Express app

// describe('POST /add', () => {
//   it('should return the sum of two numbers', async () => {
//     const response = await request(app)
//       .post('/add')
//       .send({ a: 2, b: 3 });
//     expect(response.status).toBe(200);
//     expect(response.body.result).toBe(5);
//   });

//   it('should handle invalid input', async () => {
//     const response = await request(app)
//       .post('/add')
//       .send({ a: 'foo', b: 3 });
//     expect(response.status).toBe(400);
//     expect(response.body.error).toBeDefined();
//   });
// });
