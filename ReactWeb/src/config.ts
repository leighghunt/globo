const config = {
    baseApiUrl: 'https://localhost:4000',
};

const currencyFormatter = new Intl.NumberFormat('en-NZ', {
    style: 'currency',
    currency: 'NZD',
});

export default config;
export { currencyFormatter };