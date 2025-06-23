import { PaymentStrategy } from './PaymentStrategy';
export class BankTransferPayment extends PaymentStrategy {
  pay(amount, { accountNumber, bankName }) {
    // Simulate bank transfer logic
    return `Paid $${amount} via Bank Transfer (${bankName})`;
  }
}
