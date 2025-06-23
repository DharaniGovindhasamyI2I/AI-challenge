import { legacyMultiply } from './legacyMathWrapper';

describe('legacyMultiply', () => {
  test('multiplies two positive numbers', () => {
    expect(legacyMultiply(2, 3)).toBe(6);
  });

  test('multiplies with zero', () => {
    expect(legacyMultiply(0, 5)).toBe(0);
    expect(legacyMultiply(7, 0)).toBe(0);
  });

  test('multiplies negative numbers', () => {
    expect(legacyMultiply(-2, 3)).toBe(-6);
    expect(legacyMultiply(-2, -3)).toBe(6);
  });

  test('returns 0 if either argument is null', () => {
    expect(legacyMultiply(null, 5)).toBe(0);
    expect(legacyMultiply(5, null)).toBe(0);
  });

  test('returns 0 if either argument is undefined', () => {
    expect(legacyMultiply(undefined, 5)).toBe(0);
    expect(legacyMultiply(5, undefined)).toBe(0);
  });

  test('handles non-number inputs', () => {
    expect(legacyMultiply('2', 3)).toBeNaN();
    expect(legacyMultiply({}, 3)).toBeNaN();
  });

  test('handles large numbers', () => {
    expect(legacyMultiply(1e10, 1e10)).toBe(1e20);
  });
}); 