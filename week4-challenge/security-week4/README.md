# Getting Started with Create React App

This project was bootstrapped with [Create React App](https://github.com/facebook/create-react-app).

## Available Scripts

In the project directory, you can run:

### `npm start`

Runs the app in the development mode.\
Open [http://localhost:3000](http://localhost:3000) to view it in your browser.

The page will reload when you make changes.\
You may also see any lint errors in the console.

### `npm test`

Launches the test runner in the interactive watch mode.\
See the section about [running tests](https://facebook.github.io/create-react-app/docs/running-tests) for more information.

### `npm run build`

Builds the app for production to the `build` folder.\
It correctly bundles React in production mode and optimizes the build for the best performance.

The build is minified and the filenames include the hashes.\
Your app is ready to be deployed!

See the section about [deployment](https://facebook.github.io/create-react-app/docs/deployment) for more information.

### `npm run eject`

**Note: this is a one-way operation. Once you `eject`, you can't go back!**

If you aren't satisfied with the build tool and configuration choices, you can `eject` at any time. This command will remove the single build dependency from your project.

Instead, it will copy all the configuration files and the transitive dependencies (webpack, Babel, ESLint, etc) right into your project so you have full control over them. All of the commands except `eject` will still work, but they will point to the copied scripts so you can tweak them. At this point you're on your own.

You don't have to ever use `eject`. The curated feature set is suitable for small and middle deployments, and you shouldn't feel obligated to use this feature. However we understand that this tool wouldn't be useful if you couldn't customize it when you are ready for it.

## Learn More

You can learn more in the [Create React App documentation](https://facebook.github.io/create-react-app/docs/getting-started).

To learn React, check out the [React documentation](https://reactjs.org/).

### Code Splitting

This section has moved here: [https://facebook.github.io/create-react-app/docs/code-splitting](https://facebook.github.io/create-react-app/docs/code-splitting)

### Analyzing the Bundle Size

This section has moved here: [https://facebook.github.io/create-react-app/docs/analyzing-the-bundle-size](https://facebook.github.io/create-react-app/docs/analyzing-the-bundle-size)

### Making a Progressive Web App

This section has moved here: [https://facebook.github.io/create-react-app/docs/making-a-progressive-web-app](https://facebook.github.io/create-react-app/docs/making-a-progressive-web-app)

### Advanced Configuration

This section has moved here: [https://facebook.github.io/create-react-app/docs/advanced-configuration](https://facebook.github.io/create-react-app/docs/advanced-configuration)

### Deployment

This section has moved here: [https://facebook.github.io/create-react-app/docs/deployment](https://facebook.github.io/create-react-app/docs/deployment)

### `npm run build` fails to minify

This section has moved here: [https://facebook.github.io/create-react-app/docs/troubleshooting#npm-run-build-fails-to-minify](https://facebook.github.io/create-react-app/docs/troubleshooting#npm-run-build-fails-to-minify)

# Dashboard Refactor: Before & After

## Old Code: Issues
- Duplicate imports and inconsistent exports
- Global mutable API URL
- Long, unvalidated parameter lists
- Insecure token storage (sessionStorage)
- Plain HTTP for login
- No input validation
- Memory leak in BrokenHookComponent
- Code duplication in data fetching
- Mixing hooks and class components
- Huge switch statement for simple mapping

## Security Issues
- Storing tokens in sessionStorage (XSS risk)
- Using HTTP for login (credentials exposed)
- No input validation (risk of injection)

## New Code: Improvements
- Uses React functional components and hooks
- Modular, documented API logic
- Input validation for login
- Uses HTTPS for API calls
- Stores token in localStorage (for demo; use HttpOnly cookies in production)
- No code duplication
- Proper cleanup for intervals
- Clean, consistent imports/exports
- JSDoc for main API function
- Simple value mapping

## Usage
1. Place your API at `https://localhost:3000` (update URLs as needed).
2. Run the React app as usual (`npm start`).
3. Login with a valid username and password (min 3/6 chars).
4. Fetch data after login.

## Example
```jsx
import NewDashboard, { BrokenHookComponent } from './newDashboard';

function App() {
  return <NewDashboard />;
}
```

## Note
- For real-world apps, use HttpOnly cookies for tokens and always validate/sanitize all user input.

const API_BASE_URL = 'https://localhost:3000/api/data';
/**
 * Fetches user data securely.
 * @param {string} userId
 * @param {string} token
 * @param {function} setData
 */
async function fetchUserData(userId, token, setData) {
  const url = `${API_BASE_URL}/fetch/${userId}`;
  const res = await fetch(url, { headers: { Authorization: `Bearer ${token}` } });
  const data = await res.json();
  setData(data);
}

/**
 * Fetches data from the API for a given user, with retries and callback support.
 * @param {string} userId - The user ID to fetch data for.
 * @param {string} authToken - The authentication token.
 * @param {number} retryCount - Number of retries on failure.
 * @param {function} callback - Optional callback after success.
 * @param {number} timeout - Timeout between retries in ms.
 * @param {function} setData - State setter for fetched data.
 * @returns {Promise<void>}
 * @throws {Error} If all retries fail or network error occurs.
 * @example
 * handleFetchData('123', token, 3, () => alert('done'), 1000, setData1);
 */

/**
 * Validates username and password input.
 * @param {string} username
 * @param {string} password
 * @returns {boolean}
 * @example
 * if (!validateInput(username, password)) alert('Invalid input');
 */

/**
 * Modern, secure, and maintainable Dashboard component.
 * Handles login, data fetching, and state management.
 * @component
 * @example
 * <NewDashboard />
 */

/**
 * Example component with interval and proper cleanup.
 * @component
 * @example
 * <BrokenHookComponent />
 */

try {
  await handleFetchData('789', token, 2, null, 1000, setData);
} catch (err) {
  alert('Failed to fetch data after retries.');
}

async function handleFetchData(
  userId: string,
  authToken: string,
  retryCount: number,
  callback: function | null,
  timeout: number,
  setData: function
): Promise<void>

function validateInput(
  username: string,
  password: string
): boolean

function NewDashboard(): JSX.Element
function BrokenHookComponent(): JSX.Element

// Example: Secure login and fetch
<NewDashboard />

// Example: Standalone interval counter
<BrokenHookComponent />
