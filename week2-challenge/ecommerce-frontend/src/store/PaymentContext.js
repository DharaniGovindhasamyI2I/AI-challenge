export class PaymentContext {
  setStrategy(strategy) {
    this.strategy = strategy;
  }
  pay(amount, details) {
    return this.strategy.pay(amount, details);
  }
}

// Usage
import { PaymentContext } from './PaymentContext';
import { CreditCardPayment } from './CreditCardPayment';
import { PayPalPayment } from './PayPalPayment';

const context = new PaymentContext();
context.setStrategy(new CreditCardPayment());
console.log(context.pay(100, { cardNumber: '1234', holder: 'Alice', cvv: '123' }));

context.setStrategy(new PayPalPayment());
console.log(context.pay(50, { email: 'alice@example.com' }));
