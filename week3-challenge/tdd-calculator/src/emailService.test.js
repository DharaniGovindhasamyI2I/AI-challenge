// src/emailService.test.js
jest.mock('./emailService', () => ({
  sendWelcomeEmail: jest.fn().mockResolvedValue({ messageId: '12345' })
}));
const { sendWelcomeEmail } = require('./emailService');

describe('emailService module mocks', () => {
  test('should mock sendWelcomeEmail with success', async () => {
    await expect(sendWelcomeEmail('test@example.com')).resolves.toEqual({ messageId: '12345' });
  });

  test('should mock sendWelcomeEmail with error', async () => {
    sendWelcomeEmail.mockRejectedValueOnce(new Error('Email error'));
    await expect(sendWelcomeEmail('fail@example.com')).rejects.toThrow('Email error');
  });
});