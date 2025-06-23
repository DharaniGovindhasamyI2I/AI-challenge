import { PaymentStrategy } from './PaymentStrategy';
export class PayPalPayment extends PaymentStrategy {
  pay(amount, { email }) {
    // Simulate PayPal payment logic
    return `Paid $${amount} with PayPal (${email})`;
  }
}