FROM node:16.16-alpine as build-stage

# make the '/usr/src/app' folder the current working directory
WORKDIR /usr/src/app

# copy both 'package.json' and 'package-lock.json' (if available)
COPY package*.json ./

# install project dependencies
RUN npm install

# copy project files and folders to the current working directory (i.e. '/usr/src/app' folder)
COPY . .

# Run the web service on container startup.
CMD [ "npm", "run", "start" ]