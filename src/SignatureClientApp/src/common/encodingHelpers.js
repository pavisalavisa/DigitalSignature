export const fileToBase64 = (file) => {
  const reader = new FileReader();
  reader.readAsDataURL(file);

  return new Promise((resolve) => {
    reader.onloadend = () => {
      resolve(reader.result.split(",")[1]);
    };
  });
};