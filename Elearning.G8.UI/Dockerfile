FROM node:latest as build-stage


# set working directory
WORKDIR /app
EXPOSE 3000

# add `/app/node_modules/.bin` to $PATH
ENV PATH /app/node_modules/.bin:$PATH

# install app dependencies
COPY package*.json ./
COPY package-lock.json ./
RUN npm install --silent
RUN npm install react-scripts@3.4.1 -g --silent

# add app
COPY . ./
RUN npm run build

# start app
#CMD ["npm", "start"]

FROM nginx as production-stage
WORKDIR /app
COPY --from=build-stage /app/build /app
COPY /nginx.conf /etc/nginx/nginx.conf
