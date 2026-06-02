import { WebTracerProvider } from '@opentelemetry/sdk-trace-web';
import { BatchSpanProcessor, ConsoleSpanExporter } from '@opentelemetry/sdk-trace-base';
import { OTLPTraceExporter } from '@opentelemetry/exporter-trace-otlp-http';
import { getWebAutoInstrumentations } from '@opentelemetry/auto-instrumentations-web';
import { registerInstrumentations } from '@opentelemetry/instrumentation';
import { Resource } from '@opentelemetry/resources';

const resource = new Resource({ 'service.name': 'autoverdict-web' });

// Endpoint is configurable for production deployment; defaults to local Seq instance
const otlpUrl =
  (import.meta.env.VITE_OTLP_ENDPOINT as string | undefined) ??
  'http://localhost:5341/ingest/otlp/v1/traces';

const spanProcessors = [
  new BatchSpanProcessor(new OTLPTraceExporter({ url: otlpUrl })),
];

if (import.meta.env.DEV) {
  spanProcessors.push(new BatchSpanProcessor(new ConsoleSpanExporter()));
}

const provider = new WebTracerProvider({ resource, spanProcessors });
provider.register();

registerInstrumentations({
  instrumentations: [
    getWebAutoInstrumentations({
      '@opentelemetry/instrumentation-fetch': {
        clearTimingResources: true,
      },
      '@opentelemetry/instrumentation-xml-http-request': {
        clearTimingResources: true,
      },
    }),
  ],
});
