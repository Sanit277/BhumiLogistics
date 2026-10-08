import Link from "next/link";

export default function HomePage() {
  return (
    <main>
      <section className="px-6 py-20 md:py-28 max-w-5xl mx-auto">
        <p className="text-accent-dark font-medium mb-4">
          For landowners and corporate tenants
        </p>
        <h1 className="font-display text-4xl md:text-6xl font-semibold leading-tight max-w-3xl">
          Highway-connected commercial land, verified before it's listed.
        </h1>
        <p className="text-ink-dim text-lg mt-6 max-w-2xl">
          BhumiLogistics connects landowners with warehouse and logistics
          operators looking for highway frontage — with ownership, land use, and
          setback details checked before a plot ever goes live.
        </p>

        <div className="flex gap-4 mt-10">
          <Link
            href="/listings"
            className="bg-forest text-white px-6 py-3 rounded font-medium hover:bg-forest/90"
          >
            Browse available land
          </Link>
          <Link
            href="/register"
            className="border border-hairline px-6 py-3 rounded font-medium hover:bg-surface"
          >
            List your land
          </Link>
        </div>
      </section>

      <section className="px-6 py-16 bg-surface border-y border-hairline">
        <div className="max-w-5xl mx-auto grid md:grid-cols-2 gap-12">
          <div>
            <h2 className="font-display text-2xl font-semibold mb-3">
              For landowners
            </h2>
            <p className="text-ink-dim">
              List your highway-connected land with your ownership documents on
              file. Every listing goes through an ownership and land-use review
              before it's visible to tenants — so serious operators reach out,
              not speculative inquiries.
            </p>
          </div>
          <div>
            <h2 className="font-display text-2xl font-semibold mb-3">
              For corporate tenants
            </h2>
            <p className="text-ink-dim">
              Browse verified plots with real buildable area, not just raw size
              — highway setbacks are already accounted for. Submit an offer
              directly, and track its status through to lease registration.
            </p>
          </div>
        </div>
      </section>
    </main>
  );
}
