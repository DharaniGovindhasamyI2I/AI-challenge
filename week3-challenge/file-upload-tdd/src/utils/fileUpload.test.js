import { validateFileUpload } from './fileUpload';

const makeFile = (overrides) => ({
  name: "test.txt",
  type: "text/plain",
  size: 1024,
  ...overrides,
});

const allowedTypes = ["text/plain", "image/png"];
const maxSizeMB = 1;

test("throws if no file provided", () => {
  expect(() => validateFileUpload(null, allowedTypes, maxSizeMB)).toThrow("No file provided");
});

test("throws if file type not allowed", () => {
  const file = makeFile({ type: "application/pdf" });
  expect(() => validateFileUpload(file, allowedTypes, maxSizeMB)).toThrow("Invalid file type");
});

test("throws if file size exceeds limit", () => {
  const file = makeFile({ size: 2 * 1024 * 1024 });
  expect(() => validateFileUpload(file, allowedTypes, maxSizeMB)).toThrow("File too large");
});

test("throws if file size is zero", () => {
  const file = makeFile({ size: 0 });
  expect(() => validateFileUpload(file, allowedTypes, maxSizeMB)).toThrow("File is empty");
});

test("throws if file name is missing", () => {
  const file = makeFile({ name: undefined });
  expect(() => validateFileUpload(file, allowedTypes, maxSizeMB)).toThrow();
});

test("throws if file name is too long", () => {
  const file = makeFile({ name: "a".repeat(300) + ".txt" });
  expect(() => validateFileUpload(file, allowedTypes, maxSizeMB)).toThrow();
});

test("throws if file name has invalid characters", () => {
  const file = makeFile({ name: "test/?.txt" });
  expect(() => validateFileUpload(file, allowedTypes, maxSizeMB)).toThrow();
});

test("throws if file is corrupted", () => {
  const file = makeFile({ size: NaN });
  expect(() => validateFileUpload(file, allowedTypes, maxSizeMB)).toThrow();
});

test("throws if allowedTypes is empty", () => {
  const file = makeFile();
  expect(() => validateFileUpload(file, [], maxSizeMB)).toThrow();
});

test("throws if maxSizeMB is zero", () => {
  const file = makeFile();
  expect(() => validateFileUpload(file, allowedTypes, 0)).toThrow();
});
