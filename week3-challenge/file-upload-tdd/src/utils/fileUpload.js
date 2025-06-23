export function validateFileUpload(file, allowedTypes, maxSizeMB) {
  if (!file) throw new Error("No file provided");
  if (!allowedTypes.includes(file.type)) throw new Error("Invalid file type");
  if (file.size > maxSizeMB * 1024 * 1024) throw new Error("File too large");
  if (file.size === 0) throw new Error("File is empty");
  // ...other logic
  return true;
}
