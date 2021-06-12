const isValidEmail = (email) => {
  var re = /\S+@\S+\.\S+/;

  return re.test(email);
};

const isValidPassword = (password) => {
  let digits = password.filter((c) => c >= "0" && c <= "9").length;
  let upper = password.filter((c) => c >= "A" && c <= "Z").length;
  let lower = password.filter((c) => c >= "a" && c <= "z").length;

  return digits > 1 && upper > 1 && lower > 1;
};

export default {
  isValidEmail,
  isValidPassword
};
