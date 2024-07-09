FROM node:18-alpine3.17 as build

WORKDIR /app

COPY package.json .

RUN npm install --force

COPY . .

RUN npm run build

EXPOSE 5173

CMD [ "npm", "run", "preview" ]
