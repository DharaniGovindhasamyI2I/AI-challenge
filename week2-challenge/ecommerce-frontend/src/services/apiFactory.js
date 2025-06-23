const parseJSON = async (response) => {
  const text = await response.text();
  return text ? JSON.parse(text) : {};
};

const createApiClient = (baseURL) => {
  return {
    get: (endpoint) =>
      fetch(`${baseURL}${endpoint}`)
        .then(parseJSON),
    post: (endpoint, data) =>
      fetch(`${baseURL}${endpoint}`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(data)
      }).then(parseJSON)
    // ...put, delete, etc.
  };
};

export default createApiClient;
