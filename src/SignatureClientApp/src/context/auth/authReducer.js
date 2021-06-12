export default (state, action) => {
    switch (action.type) {
      case 'USER_LOADED':
        return {
          ...state,
          isAuthenticated: true,
          loading: false,
          user: action.payload,
        };
      case 'LOGIN_SUCCESS':
        return {
          ...state,
          ...action.payload,
          isAuthenticated: true,
          loading: false,
        };
      case 'AUTH_ERROR':
      case 'LOGIN_FAIL':
        return {
          ...state,
          token: null,
          isAuthenticated: false,
          loading: false,
          user: null,
          error: action.payload,
        };
      default:
        return state;
    }
  };
  