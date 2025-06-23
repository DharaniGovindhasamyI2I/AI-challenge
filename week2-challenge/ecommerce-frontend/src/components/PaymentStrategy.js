export class PaymentStrategy {
  pay(amount, details) {
    throw new Error('pay() must be implemented');
  }
}
