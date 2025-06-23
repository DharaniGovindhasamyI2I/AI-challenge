// legacyMath.js
function legacyMultiply(a, b) {
  if (a == null || b == null) return 0;
  if (typeof a !== 'number' || typeof b !== 'number') return NaN;
  return a * b;
}

module.exports = { legacyMultiply }; 