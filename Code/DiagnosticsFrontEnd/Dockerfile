FROM node:18.13 
COPY package.json package-lock.json ./
RUN npm ci && mkdir /angular && mv ./node_modules ./angular

WORKDIR /angular
RUN npm install -g @angular/cli 
COPY . . 

CMD ["npm", "run", "start"]
