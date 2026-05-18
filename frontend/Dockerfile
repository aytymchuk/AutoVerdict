# Build Stage
FROM node:22-alpine AS build

WORKDIR /app

# Install pnpm globally
RUN npm install -g pnpm

COPY package.json pnpm-lock.yaml* pnpm-workspace.yaml* ./
RUN pnpm install --frozen-lockfile

COPY . .

# Declare build arg so Vite can embed it into the bundle at build time
ARG VITE_CLERK_PUBLISHABLE_KEY
ENV VITE_CLERK_PUBLISHABLE_KEY=${VITE_CLERK_PUBLISHABLE_KEY}

# Build the application
RUN pnpm run build

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
CMD ["serve", "-s", "dist", "-l", "3000"]
