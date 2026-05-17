import { WebTracerProvider } from '@opentelemetry/sdk-trace-web';
import { BatchSpanProcessor, ConsoleSpanExporter } from '@opentelemetry/sdk-trace-base';
import { getWebAutoInstrumentations } from '@opentelemetry/auto-instrumentations-web';
import { registerInstrumentations } from '@opentelemetry/instrumentation';

const provider = new WebTracerProvider({
  spanProcessors: [
    new BatchSpanProcessor(new ConsoleSpanExporter())
  ]
});

provider.register();

registerInstrumentations({
  instrumentations: [
    getWebAutoInstrumentations({
      '@opentelemetry/instrumentation-fetch': {
        // Optional configuration for fetch instrumentation
        clearTimingResources: true,
      },
      '@opentelemetry/instrumentation-xml-http-request': {
        clearTimingResources: true,
      },
    }),
  ],
});
