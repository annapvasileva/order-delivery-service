FROM node:22

WORKDIR /app

COPY order-delivery-frontend/package*.json ./order-delivery-frontend/

WORKDIR /app/order-delivery-frontend

RUN npm install

COPY order-delivery-frontend ./

RUN npm run build

CMD ["node", "dist/main.js"]