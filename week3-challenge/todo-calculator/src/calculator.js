import React, { useState } from 'react';
import { add } from './add';
import './calculator.css';

function Calculator() {
  const [a, setA] = useState('');
  const [b, setB] = useState('');
  const [result, setResult] = useState(null);

  const handleAdd = () => {
    // Convert input values to numbers before adding
    setResult(add(Number(a), Number(b)));
  };

  return (
    <div className='calculator'>
      <h2>Simple Calculator</h2>
      <input className='input'
        type="number"
        value={a}
        onChange={e => setA(e.target.value)}
        placeholder="First number"
      />
      <input className='input'
        type="number"
        value={b}
        onChange={e => setB(e.target.value)}
        placeholder="Second number"
      />
      <button className='button' type='submit'  onClick={handleAdd}>Add</button>
      {result !== null && (
        <div>
          <strong>Result: {result}</strong>
        </div>
      )}
    </div>
  );
}

export default Calculator;
