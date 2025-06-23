/**
 * Adds two numbers together
 * @param {number} a - First number
 * @param {number} b - Second number
 * @returns {number} Sum of a and b
 * @throws {Error} If inputs are not numbers or are NaN
 */
export function add(a, b) {
  if (typeof a !== 'number' || typeof b !== 'number') {
    throw new Error('Both arguments must be numbers');
  }
  if (isNaN(a) || isNaN(b)) {
    throw new Error('Arguments cannot be NaN');
  }
  return a + b;
} 