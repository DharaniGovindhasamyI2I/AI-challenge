import { PaymentStrategy } from './PaymentStrategy';
export class CreditCardPayment extends PaymentStrategy {
  pay(amount, { cardNumber, holder, cvv }) {
    // Simulate credit card payment logic
    return `Paid $${amount} with Credit Card (${cardNumber})`;
  }
}
