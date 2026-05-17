# Build Stage
FROM node:22-alpine AS build

WORKDIR /app

COPY package.json package-lock.json* npm-shrinkwrap.json* ./
# Use npm ci if lockfile exists, else npm install
RUN if [ -f package-lock.json ]; then npm ci; else npm install; fi

COPY . .

# Build the application
RUN npm run build

# Production Stage
FROM node:22-alpine AS production

WORKDIR /app

# Drop root privileges for security
RUN addgroup -g 1001 -S nodejs && \
    adduser -S nodejs -u 1001

# Install serve to host the static files
RUN npm install -g serve

# Copy built assets from the build stage
COPY --from=build --chown=nodejs:nodejs /app/dist ./dist

# Switch to the non-root user
USER nodejs

# Expose port 3000
EXPOSE 3000

# Start the static server
CMD ["npx", "serve", "-s", "dist", "-l", "3000"]
